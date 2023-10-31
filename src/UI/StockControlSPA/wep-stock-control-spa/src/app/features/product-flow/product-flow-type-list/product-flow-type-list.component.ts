import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { productFlowUrls } from '../product-flow-routing.module';
import { IPagination } from 'src/app/shared/modules/pagination/interfaces/pagination.intarface';

@Component({
  selector: 'app-product-flow-type-list',
  templateUrl: './product-flow-type-list.component.html',
  styleUrls: ['./product-flow-type-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProductFlowTypeListComponent implements OnInit, OnDestroy  {
  private readonly destroy$ = new Subject<void>();
  
  selectedIndex = 0;

  paginationInfo = {} as IPagination;

  pageChanged = new Subject<number>();

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute){
  }

  ngOnInit(): void {
    this.load();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onChangedSelectedIndex(event: any) : void {
    let path = '';
    switch(event) {
      case 0:
        path = `${productFlowUrls.receipts}`;
        break;
      case 1: 
        path = `${productFlowUrls.movings}`;
        break;
      case 2:
        path = `${productFlowUrls.writeOffs}`;
        break;
    }
    this.selectedIndex = event;
    this.router.navigate([`${path}`], {
      relativeTo: this.activatedRoute,
      queryParamsHandling: ""
    });
  }

  onPageChanged(value: any): void {
    this.paginationInfo.page = value;
    this.pageChanged.next(this.paginationInfo.page);
  }

  onPaginationInfoChanged(event: IPagination) {
    this.paginationInfo = {...event};
  }

  private load() {
    if (this.router.url.includes(`${productFlowUrls.movings}`)) {
      this.selectedIndex = 1;
    }
    else if (this.router.url.includes(`${productFlowUrls.writeOffs}`)) {
      this.selectedIndex = 2;
    }
    else {
      this.selectedIndex = 0;
    }
    this.onChangedSelectedIndex(this.selectedIndex);
  }
}
