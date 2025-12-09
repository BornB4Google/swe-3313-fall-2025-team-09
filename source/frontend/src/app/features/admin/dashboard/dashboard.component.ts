import { Component, inject, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { Chart, registerables } from 'chart.js';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import {
  RecentSale,
  RevenueReport,
  OrderSummary,
  SoldItem,
  SaleReportItem,
} from '../../../models/reports.model';
import { OrderDetailDto } from '../../../models/order.models';
import { ReportsService } from '../../../services/report/report.service';
import { SaleService } from '../../../services/sales/sale.service';
import { ReceiptModalComponent } from '../../../shared/receipt-modal/receipt-modal.component';

Chart.register(...registerables);

const compactCurrencyFormatter = new Intl.NumberFormat('en-US', {
  style: 'currency',
  currency: 'USD',
  notation: 'compact',
  maximumFractionDigits: 1,
});

const usRegions = {
  Northeast: ['CT', 'ME', 'MA', 'NH', 'RI', 'VT', 'NJ', 'NY', 'PA'],
  Midwest: ['IL', 'IN', 'MI', 'OH', 'WI', 'IA', 'KS', 'MN', 'MO', 'NE', 'ND', 'SD'],
  South: [
    'DE',
    'FL',
    'GA',
    'MD',
    'NC',
    'SC',
    'VA',
    'WV',
    'AL',
    'KY',
    'MS',
    'TN',
    'AR',
    'LA',
    'OK',
    'TX',
  ],
  West: ['AZ', 'CO', 'ID', 'MT', 'NV', 'NM', 'UT', 'WY', 'AK', 'CA', 'HI', 'OR', 'WA'],
};

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, ReceiptModalComponent],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit, AfterViewInit {
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

  @ViewChild('monthlyRevenueChart') monthlyRevenueChartRef!: ElementRef<HTMLCanvasElement>;
  @ViewChild('regionChart') regionChartRef!: ElementRef<HTMLCanvasElement>;

  private monthlyChart: Chart | null = null;
  private regionChart: Chart | null = null;
  private salesData: SaleReportItem[] = [];

  private saleService = inject(SaleService);
  private reportsService = inject(ReportsService);

  ngOnInit(): void {
    this.loadDashboardData();
  }

  ngAfterViewInit(): void {
    this.loadChartsData();
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

  private loadChartsData(): void {
    this.reportsService.getSalesReport().subscribe((sales: SaleReportItem[]) => {
      this.salesData = sales;
      this.renderMonthlyRevenueChart();
      this.renderRegionChart();
    });
  }

  private renderMonthlyRevenueChart(): void {
    const monthlyData = this.aggregateMonthlyRevenue(this.salesData);
    const labels = monthlyData.map(d => d.month);
    const revenueData = monthlyData.map(d => d.revenue);
    const totalRevenue = revenueData.reduce((sum, val) => sum + val, 0);
    const percentageData = revenueData.map(rev =>
      totalRevenue > 0 ? (rev / totalRevenue) * 100 : 0
    );

    if (this.monthlyChart) {
      this.monthlyChart.destroy();
    }

    this.monthlyChart = new Chart(this.monthlyRevenueChartRef.nativeElement, {
      type: 'bar',
      data: {
        labels,
        datasets: [
          {
            label: 'Revenue ($)',
            data: revenueData,
            backgroundColor: '#a7b3bc',
            borderColor: '#c0ccd5',
            borderWidth: 1,
            yAxisID: 'y',
            order: 2,
          },
          {
            label: '% of Total',
            data: percentageData,
            type: 'line',
            borderColor: '#5a8a9a',
            backgroundColor: '#5a8a9a',
            borderWidth: 2,
            pointRadius: 4,
            pointBackgroundColor: '#5a8a9a',
            yAxisID: 'y1',
            order: 1,
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            labels: { color: '#f4f7f9' },
          },
        },
        scales: {
          x: {
            ticks: { color: '#a7b3bc' },
            grid: { color: 'rgba(159,170,179,0.2)' },
          },
          y: {
            type: 'linear',
            position: 'left',
            ticks: {
              color: '#a7b3bc',
              callback: (value: number | string) => {
                const num = typeof value === 'string' ? parseFloat(value) : value;
                if (isNaN(num)) {
                  return '';
                }
                return compactCurrencyFormatter.format(num);
              },
            },
            grid: { color: 'rgba(159,170,179,0.2)' },
          },
          y1: {
            type: 'linear',
            position: 'right',
            min: 0,
            max: 100,
            ticks: {
              color: '#5a8a9a',
              callback: (value: number | string) => `${value}%`,
            },
            grid: { display: false },
          },
        },
      },
    });
  }

  private renderRegionChart(): void {
    const regionData = this.aggregateSalesByRegion(this.salesData);
    const labels = Object.keys(regionData);
    const data = Object.values(regionData);

    if (this.regionChart) {
      this.regionChart.destroy();
    }

    this.regionChart = new Chart(this.regionChartRef.nativeElement, {
      type: 'pie',
      data: {
        labels,
        datasets: [
          {
            label: 'Sales by Region ($)',
            data,
            backgroundColor: ['#122336', '#4f6b8c', '#273c54', '#658998'],
            borderColor: ['#1a3048', '#5f7b9c', '#374c64', '#75a9a8'],
            borderWidth: 1,
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            position: 'right',
            labels: { color: '#f4f7f9' },
          },
        },
      },
    });
  }

  private aggregateMonthlyRevenue(sales: SaleReportItem[]): { month: string; revenue: number }[] {
    const monthMap = new Map<string, { sortKey: string; label: string; revenue: number }>();
    const monthNames = [
      'Jan',
      'Feb',
      'Mar',
      'Apr',
      'May',
      'Jun',
      'Jul',
      'Aug',
      'Sep',
      'Oct',
      'Nov',
      'Dec',
    ];

    for (const sale of sales) {
      const date = new Date(sale.saleDateTime);
      const sortKey = `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}`;
      const label = `${monthNames[date.getMonth()]} '${String(date.getFullYear()).slice(-2)}`;
      const existing = monthMap.get(sortKey);
      monthMap.set(sortKey, {
        sortKey,
        label,
        revenue: (existing?.revenue || 0) + sale.total,
      });
    }

    return Array.from(monthMap.values())
      .sort((a, b) => a.sortKey.localeCompare(b.sortKey))
      .map(({ label, revenue }) => ({ month: label, revenue }));
  }

  private aggregateSalesByRegion(sales: SaleReportItem[]): Record<string, number> {
    const regionTotals: Record<string, number> = {
      Northeast: 0,
      Midwest: 0,
      South: 0,
      West: 0,
      Other: 0,
    };

    for (const sale of sales) {
      const state = sale.state?.toUpperCase();
      let assigned = false;

      for (const [region, states] of Object.entries(usRegions)) {
        if (states.includes(state)) {
          regionTotals[region] += sale.total;
          assigned = true;
          break;
        }
      }

      if (!assigned) {
        regionTotals['Other'] += sale.total;
      }
    }

    // remove other if empty
    if (regionTotals['Other'] === 0) {
      delete regionTotals['Other'];
    }

    return regionTotals;
  }
}
