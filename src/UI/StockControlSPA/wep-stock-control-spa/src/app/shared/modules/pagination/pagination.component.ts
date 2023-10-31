import { Component, Input, OnChanges, OnInit, Output } from '@angular/core';
import { Subject } from 'rxjs';
import { IPagination } from './interfaces/pagination.intarface';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss']
})

export class PaginationComponent implements OnInit, OnChanges {

  @Output()
  changed = new Subject<number>();

  @Input()
  model: IPagination;

  buttonStates: any = {
      nextDisabled: true,
      previousDisabled: true,
  };

  constructor(){
  }

  ngOnInit() {
  }

  ngOnChanges() {
    if (this.model) {
      this.model.items = (this.model.items > this.model.totalItems) ? this.model.totalItems : this.model.items;
        
      this.buttonStates.previousDisabled = (this.model.page <= 1);
      this.buttonStates.nextDisabled = (this.model.page >= this.model.totalPages);
    }
  }

  onNextClicked(event: any) {
      event.preventDefault();
      this.changed.next(this.model.page + 1);
  }

  onPreviousCliked(event: any) {
      event.preventDefault();
      this.changed.next(this.model.page - 1);
  }
}
