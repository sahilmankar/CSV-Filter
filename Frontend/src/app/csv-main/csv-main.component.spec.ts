import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CsvMainComponent } from './csv-main.component';

describe('CsvMainComponent', () => {
  let component: CsvMainComponent;
  let fixture: ComponentFixture<CsvMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CsvMainComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CsvMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
