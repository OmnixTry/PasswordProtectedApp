import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrdVerificationFromComponent } from './crd-verification-from.component';

describe('CrdVerificationFromComponent', () => {
  let component: CrdVerificationFromComponent;
  let fixture: ComponentFixture<CrdVerificationFromComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CrdVerificationFromComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CrdVerificationFromComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
