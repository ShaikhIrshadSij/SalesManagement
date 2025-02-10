import { Component, OnInit } from "@angular/core"
import { CarModelService } from '../../services/car-model.service'
import { CarModel } from '../../models/car-model.model'

@Component({
  selector: "app-car-model-list",
  templateUrl: "./car-model-list.component.html",
  styleUrls: ["./car-model-list.component.scss"],
})
export class CarModelListComponent implements OnInit {
  carModels: CarModel[] = []
  searchTerm = ""

  constructor(private carModelService: CarModelService) { }

  ngOnInit(): void {
    this.loadCarModels()
  }

  loadCarModels(): void {
    this.carModelService.getAllModels().subscribe((models) => (this.carModels = models))
  }

  searchModels(): void {
    if (this.searchTerm.trim()) {
      this.carModelService.searchModels(this.searchTerm).subscribe((models) => (this.carModels = models))
    } else {
      this.loadCarModels()
    }
  }

  deleteModel(id: number): void {
    if (confirm("Are you sure you want to delete this model?")) {
      this.carModelService.deleteModel(id).subscribe(() => {
        this.carModels = this.carModels.filter((model) => model.modelId !== id)
      })
    }
  }

  setDefaultImage(event: any) {
    event.target.src = 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQjszKShiN_uknU5Lxn694iMkXao8sl7JtpVg&s';
  }
}

