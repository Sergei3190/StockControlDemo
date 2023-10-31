import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-page-no-content',
  templateUrl: './page-no-content.component.html',
  styleUrls: ['./page-no-content.component.scss']
})
export class PageNoContentComponent implements OnInit {

  @Input()
  load:() => void;

  constructor() { }

  ngOnInit() {
  }

  return(){
    this.load();
  }
}
