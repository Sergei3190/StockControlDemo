import { Component, Input, OnInit, Output } from '@angular/core';
import { QueryParamsHandling } from '@angular/router';
import { Subject } from 'rxjs';
import { SideBarItem } from './models/sidebar-item.model';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})

export class SideBarComponent implements OnInit {
  @Input()
  tabs?: SideBarItem[] = [];

  @Input()
  userName = "";

  @Input()
  queryParamsHandling: QueryParamsHandling = '';

  @Output() 
  signOut = new Subject<void>();

  @Output() 
  signIn = new Subject<void>();

  constructor(){
  }

  ngOnInit(): void {
  }

  login(){
    this.signIn.next();  
  }

  logout(){
    this.signOut.next();  
  }
}
