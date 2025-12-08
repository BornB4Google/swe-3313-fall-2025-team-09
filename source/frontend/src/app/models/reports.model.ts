export interface SaleReportItem {
  saleId: number;
  userId: number;
  saleDateTime: string;
  subtotal: number;
  tax: number;
  shippingCost: string;
  total: number;
  shippingSpeed: string;
  street1: string;
  street2: string | null;
  city: string;
  state: string;
  zip: string;
  cardLast4: string;
}

export interface RevenueSummary {
  orderCount: number;
  subtotal: number;
  tax: number;
  shipping: number;
  totalRevenue: number;
}

export interface RevenueByDay {
  date: string;
  orderCount: number;
  subtotal: number;
  tax: number;
  shipping: number;
  total: number;
}

export interface RevenueReport {
  summary: RevenueSummary;
  byDay: RevenueByDay[];
}

export interface WeeklyDataPoint {
  weekStart: string;
  weekEnd: string;
  orderCount: number;
  total: number;
  subtotal: number;
  tax: number;
  shipping: number;
}

export interface WeeklyReportSummary {
  totalOrders: number;
  totalRevenue: number;
  totalSubtotal: number;
  totalTax: number;
  totalShipping: number;
}

export interface WeeklyReport {
  summary: WeeklyReportSummary;
  weeklyDataPoints: WeeklyDataPoint[];
}

export interface MonthlyDataPoint {
  Month: number;
  MonthStart: string;
  MonthEnd: string;
  OrderCount: number;
  Total: number;
  Subtotal: number;
  Tax: number;
  Shipping: number;
}

export interface MonthlyReportSummary {
  TotalOrders: number;
  TotalRevenue: number;
  TotalSubtotal: number;
  TotalTax: number;
  TotalShipping: number;
}

export interface MonthlyReport {
  Summary: MonthlyReportSummary;
  MonthlyDataPoints: MonthlyDataPoint[];
}

export interface RecentSale {
  itemName: string;
  itemDescription: string;
  dateSold: string;
  lineTotal: number;
  saleId: number;
}

export interface OrderSummary {
  saleId: number;
  saleDateTime: string;
  total: number;
  itemCount: number;
  userId: number;
  customerName: string;
  customerEmail: string;
}

export interface SoldItem {
  itemId: number;
  itemName: string;
  itemDescription: string;
  dateSold: string;
  lineTotal: number;
  saleId: number;
}
