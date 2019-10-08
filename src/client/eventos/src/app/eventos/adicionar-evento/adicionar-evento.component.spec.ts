import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdicionarEventoComponent } from './adicionar-evento.component';

describe('AdicionarEventoComponent', () => {
  let component: AdicionarEventoComponent;
  let fixture: ComponentFixture<AdicionarEventoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdicionarEventoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdicionarEventoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
