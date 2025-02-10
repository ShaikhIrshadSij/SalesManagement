import { Component, OnInit } from "@angular/core"
import { SalesmanCommissionReport } from '../../models/salesman-commission-report.model'
import { ReportService } from '../../services/report.service'

@Component({
  selector: "app-salesman-report",
  templateUrl: "./salesman-report.component.html",
  styleUrls: ["./salesman-report.component.scss"],
})
export class SalesmanReportComponent implements OnInit {
  salesmanReports: SalesmanCommissionReport[] = []

  constructor(private reportService: ReportService) { }

  ngOnInit(): void {
    this.loadSalesmanReports()
  }

  loadSalesmanReports(): void {
    this.reportService.getSalesmanCommissionReport().subscribe((reports) => (this.salesmanReports = reports))
  }
}

