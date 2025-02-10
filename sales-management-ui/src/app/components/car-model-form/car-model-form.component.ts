import { Component, OnInit } from "@angular/core"
import { FormBuilder, FormGroup, Validators } from "@angular/forms"
import { ActivatedRoute, Router } from "@angular/router"
import { CarModelService } from '../../services/car-model.service'
import { CarModel } from '../../models/car-model.model'

@Component({
  selector: "app-car-model-form",
  templateUrl: "./car-model-form.component.html",
  styleUrls: ["./car-model-form.component.scss"],
})
export class CarModelFormComponent implements OnInit {
  carModelForm: FormGroup
  isEditMode = false
  modelId: number | null = null

  constructor(
    private fb: FormBuilder,
    private carModelService: CarModelService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.carModelForm = this.fb.group({
      brand: ["", Validators.required],
      class: ["", Validators.required],
      modelName: ["", Validators.required],
      modelCode: ["", [Validators.required]],
      description: [""],
      features: [""],
      price: [""],
      dateOfManufacturing: [""],
      isActive: [true],
      sortOrder: [""],
      imagePaths: [""],
    })
  }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      if (params["id"]) {
        this.isEditMode = true
        this.modelId = +params["id"]
        this.loadCarModel(this.modelId)
      }
    })
  }

  loadCarModel(id: number): void {
    this.carModelService.getModelById(id).subscribe((model) => {
      this.carModelForm.patchValue(model)
    })
  }

  onSubmit(): void {
    if (this.carModelForm.valid) {
      const carModel: any = this.carModelForm.value
      carModel.imagePaths = Array.isArray(this.carModelForm.value.imagePaths) ? this.carModelForm.value.imagePaths : this.carModelForm.value.imagePaths?.split(',')
      carModel.imagePaths = carModel.imagePaths.filter((x: any) => x)
      Object.keys(carModel).forEach(item => {
        if (!carModel[item]) {
          carModel[item] = null
        }
      })
      if (this.isEditMode && this.modelId) {
        this.carModelService.updateModel(this.modelId, carModel).subscribe(() => {
          this.router.navigate(["/car-models"])
        })
      } else {
        this.carModelService.createModel(carModel).subscribe(() => {
          this.router.navigate(["/car-models"])
        })
      }
    }
  }
}

