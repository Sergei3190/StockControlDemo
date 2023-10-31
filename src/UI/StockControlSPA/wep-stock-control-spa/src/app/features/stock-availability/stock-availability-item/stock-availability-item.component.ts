import { Component, OnInit } from '@angular/core';
import { SideDrawerBaseComponent } from 'src/app/shared/modules/side-drawer/side-drawer-base/side-drawer-base.component';
import { IStockAvailabilityItem } from '../interfaces/stock-availability-item.interface';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-stock-availability-item',
  templateUrl: './stock-availability-item.component.html',
  styleUrls: ['./stock-availability-item.component.scss']
})
export class StockAvailabilityItemComponent extends SideDrawerBaseComponent<StockAvailabilityItemComponent> implements OnInit {

  title: string;
  model: IStockAvailabilityItem;
  form: FormGroup;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private fb: FormBuilder) {
      super();
  }

  ngOnInit(): void {
    this.initProperties();
    this.setStartRoute();
  }

  private initProperties() {
    this.title = this.data.title;
    this.model = this.data.model;
    this.form = this.fb.group({
      id: [{ value: this.model.id, disabled: true }],
      price: [{ value: this.model.price, disabled: true }],
      quantity: [{ value: this.model.quantity, disabled: true }],
      totalPrice: [{ value: this.model.totalPrice, disabled: true }],
      receiptId: [{ value: this.model.receiptId, disabled: true }],
      movingId: [{ value: this.model.movingId, disabled: true }],
      writeOffId: [{ value: this.model.writeOffId, disabled: true }],
      party: this.fb.group({
        id: [{ value: this.model.party?.id, disabled: true }],
        number: [{ value: this.model.party?.number, disabled: true }],
        extensionNumber: [{ value: this.model.party?.extensionNumber, disabled: true }],
        createDate: [{ value: this.model.party?.createDate, disabled: true }],
        createTime: [{ value: this.model.party?.createTime, disabled: true }],
      }),
      nomenclature: this.fb.group({
        id: [{ value: this.model.nomenclature?.id, disabled: true }],
        name: [{ value: this.model.nomenclature?.name, disabled: true }],
      }), 
      warehouse: this.fb.group({
        id: [{ value: this.model.warehouse?.id, disabled: true }],
        name: [{ value: this.model.warehouse?.name, disabled: true }],
      }),
      organization: this.fb.group({
        id: [{ value: this.model.organization?.id, disabled: true }],
        name: [{ value: this.model.organization?.name, disabled: true }],
      })
    });
  }

  private setStartRoute() {
    this.router.navigate(['./', this.model.id], {
      relativeTo: this.activatedRoute,
    });
  }
}
