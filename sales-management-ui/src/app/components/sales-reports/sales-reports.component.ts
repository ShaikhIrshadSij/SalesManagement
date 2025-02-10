import { Component, OnInit } from '@angular/core';
import { ReportService } from '../../services/report.service';
import { SalesmanCommissionReport } from '../../models/salesman-commission-report.model';
import { SalesByBrandReport } from '../../models/sales-by-brand-report.model';
import { SalesByClassReport } from '../../models/sales-by-class-report.model';
import { TopSellingModelReport } from '../../models/top-selling-model-report.model';

@Component({
  selector: 'app-sales-reports',
  templateUrl: './sales-reports.component.html',
  styleUrls: ['./sales-reports.component.scss']
})
export class SalesReportsComponent implements OnInit {
  salesmanCommissionReports: SalesmanCommissionReport[] = [];
  salesByBrandReports: SalesByBrandReport[] = [];
  salesByClassReports: SalesByClassReport[] = [];
  topSellingModelReports: TopSellingModelReport[] = [];

  constructor(private reportService: ReportService) { }

  ngOnInit(): void {
    this.loadReports();
  }

  loadReports(): void {
    this.reportService.getSalesmanCommissionReport().subscribe(
      reports => this.salesmanCommissionReports = reports
    );

    this.reportService.getSalesByBrandReport().subscribe(
      reports => this.salesByBrandReports = reports
    );

    this.reportService.getSalesByClassReport().subscribe(
      reports => this.salesByClassReports = reports
    );

    this.reportService.getTopSellingModelsReport().subscribe(
      reports => this.topSellingModelReports = reports
    );
  }
}