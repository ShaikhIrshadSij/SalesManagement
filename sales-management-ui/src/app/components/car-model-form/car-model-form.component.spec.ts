import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CarModelFormComponent } from './car-model-form.component';

describe('CarModelFormComponent', () => {
  let component: CarModelFormComponent;
  let fixture: ComponentFixture<CarModelFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CarModelFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CarModelFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
