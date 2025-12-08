import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import {
  RecentSale,
  WeeklyReport,
  RevenueReport,
  MonthlyReport,
  OrderSummary,
  SoldItem,
} from '../../../models/reports.model';
import { OrderDetailDto } from '../../../models/order.models';
import { ReportsService } from '../../../services/report/report.service';
import { SaleService } from '../../../services/sales/sale.service';
import { ReceiptModalComponent } from '../../../shared/receipt-modal/receipt-modal.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, ReceiptModalComponent],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  totalOrders = 0;
  totalRevenue = 0;

  recentSales: RecentSale[] = [];

  showReceiptModal = false;
  showItemsModal = false;

  weeklyStartDate: string = new Date().toISOString().split('T')[0];
  monthlyYear: number = new Date().getFullYear();
  monthlyMonth: number = new Date().getMonth() + 1;

  receiptSaleId!: number;
  receiptEmail!: string;
  receiptStart!: string;
  receiptEnd!: string;
  receiptResults: OrderSummary[] = [];

  itemName!: string;
  itemId!: number;
  itemStart!: string;
  itemEnd!: string;
  itemResults: SoldItem[] = [];

  loading = true;
  selectedReceipt: OrderDetailDto | null = null;

  private saleService = inject(SaleService);

  constructor(private reportsService: ReportsService) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.loading = true;

    this.reportsService.getRevenueReport().subscribe((report: RevenueReport) => {
      this.totalOrders = report.summary.orderCount;
      this.totalRevenue = report.summary.totalRevenue;
    });

    this.reportsService.getRecentSales().subscribe((items: RecentSale[]) => {
      this.recentSales = items;
      this.loading = false;
    });
  }

  downloadWeeklyCsv(): void {
    if (!this.weeklyStartDate) return;

    const start = new Date(this.weeklyStartDate);

    if (isNaN(start.getTime())) return;

    this.reportsService
      .downloadWeeklyCsv(start)
      .subscribe(file => this.saveCsv(file, `weekly-report-${this.weeklyStartDate}.csv`));
  }

  downloadMonthlyCsv(): void {
    if (!this.monthlyYear || !this.monthlyMonth) return;

    this.reportsService.downloadMonthlyCsv(this.monthlyYear, this.monthlyMonth).subscribe(file => {
      const name = `monthly-report-${this.monthlyYear}-${this.monthlyMonth}.csv`;
      this.saveCsv(file, name);
    });
  }

  searchReceipts(): void {
    const start = this.receiptStart ? new Date(this.receiptStart) : undefined;
    const end = this.receiptEnd ? new Date(this.receiptEnd) : undefined;

    this.reportsService
      .searchOrders(
        this.receiptSaleId || undefined,
        this.receiptEmail || undefined,
        isNaN(start?.getTime() ?? NaN) ? undefined : start,
        isNaN(end?.getTime() ?? NaN) ? undefined : end
      )
      .subscribe(results => (this.receiptResults = results));
  }

  searchItems(): void {
    const start = this.itemStart ? new Date(this.itemStart) : undefined;
    const end = this.itemEnd ? new Date(this.itemEnd) : undefined;

    this.reportsService
      .searchSoldItems(
        this.itemName || undefined,
        this.itemId || undefined,
        isNaN(start?.getTime() ?? NaN) ? undefined : start,
        isNaN(end?.getTime() ?? NaN) ? undefined : end
      )
      .subscribe(results => (this.itemResults = results));
  }

  private saveCsv(file: Blob, fileName: string): void {
    const url = window.URL.createObjectURL(file);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    window.URL.revokeObjectURL(url);
  }

  openReceipt(saleId: number): void {
    this.saleService.getOrderById(saleId).subscribe({
      next: orderDetail => (this.selectedReceipt = orderDetail),
      error: err => console.error('Failed to load order details:', err),
    });
  }

  closeReceipt(): void {
    this.selectedReceipt = null;
  }
}
