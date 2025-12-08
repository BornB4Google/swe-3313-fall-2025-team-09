import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  SaleReportItem,
  RevenueReport,
  WeeklyReport,
  MonthlyReport,
  RecentSale,
  OrderSummary,
  SoldItem
} from '../../models/reports.model';


@Injectable({
  providedIn: 'root'
})
export class ReportsService {

  constructor(private http: HttpClient) {}


  getSalesReport(startDate?: Date, endDate?: Date): Observable<SaleReportItem[]> {
    let params = new HttpParams();
    if (startDate) params = params.set('startDate', startDate.toISOString());
    if (endDate) params = params.set('endDate', endDate.toISOString());
    return this.http.get<SaleReportItem[]>(`/api/reports/sales`, { params });
  }


  getRevenueReport(startDate?: Date, endDate?: Date): Observable<RevenueReport> {
    let params = new HttpParams();
    if (startDate) params = params.set('startDate', startDate.toISOString());
    if (endDate) params = params.set('endDate', endDate.toISOString());
    return this.http.get<RevenueReport>(`/api/reports/revenue`, { params });
  }


  getWeeklySalesReport(startDate: Date): Observable<WeeklyReport> {
    const params = new HttpParams().set('startDate', startDate.toISOString());
    return this.http.get<WeeklyReport>(`/api/reports/sales/weekly`, { params });
  }


  downloadWeeklyCsv(startDate: Date): Observable<Blob> {
    const params = new HttpParams().set('startDate', startDate.toISOString());
    return this.http.get(`/api/reports/sales/weekly/csv`, {
      params,
      responseType: 'blob'
    });
  }

  getMonthlySalesReport(year?: number, month?: number): Observable<MonthlyReport> {
    let params = new HttpParams();
    if (year) params = params.set('year', year.toString());
    if (month) params = params.set('month', month.toString());
    return this.http.get<MonthlyReport>(`/api/reports/sales/monthly`, { params });
  }

  downloadMonthlyCsv(year: number, month: number): Observable<Blob> {
    const params = new HttpParams()
      .set('year', year)
      .set('month', month);
    return this.http.get(`/api/reports/sales/monthly/csv`, {
      params,
      responseType: 'blob'
    });
  }

  getRecentSales(): Observable<RecentSale[]> {
    return this.http.get<RecentSale[]>(`/api/reports/recent-sales`);
  }

  searchOrders(
    saleId?: number,
    email?: string,
    startDate?: Date,
    endDate?: Date
  ): Observable<OrderSummary[]> {

    let params = new HttpParams();
    if (saleId) params = params.set('saleId', saleId.toString());
    if (email) params = params.set('customerEmail', email);
    if (startDate) params = params.set('startDate', startDate.toISOString());
    if (endDate) params = params.set('endDate', endDate.toISOString());
    return this.http.get<OrderSummary[]>(`/api/reports/search`, { params });
  }

  searchSoldItems(
    name?: string,
    itemId?: number,
    startDate?: Date,
    endDate?: Date
  ): Observable<SoldItem[]> {

    let params = new HttpParams();

    if (name) params = params.set('name', name);
    if (itemId) params = params.set('itemId', itemId.toString());
    if (startDate) params = params.set('startDate', startDate.toISOString());
    if (endDate) params = params.set('endDate', endDate.toISOString());

    return this.http.get<SoldItem[]>(`/api/reports/sold-items/search`, { params });
  }
}
