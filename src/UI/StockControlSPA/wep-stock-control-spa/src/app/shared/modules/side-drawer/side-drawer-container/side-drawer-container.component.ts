import { Component, ComponentRef, Input, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { MatDrawer } from '@angular/material/sidenav';
import { ISideDrawerContainerConfig } from '../interfaces/side-drawer-container-config.interface';
import { ISideDrawerConfig } from '../interfaces/side-drawer-config.interface';

@Component({
  selector: 'app-side-drawer-container, [app-side-drawer-container]', 
  templateUrl: './side-drawer-container.component.html',
  styleUrls: ['./side-drawer-container.component.scss']
})

export class SideDrawerContainerComponent implements OnInit {

  @Input()
  config: ISideDrawerContainerConfig;

  @ViewChild('drawer', { static: true }) drawer: MatDrawer;
  @ViewChild('drawerItem', {read: ViewContainerRef}) drawerItem: ViewContainerRef;

  constructor(){}

  ngOnInit(): void {
    this.initConfig();
  }

  openDrawer(containerConfig: ISideDrawerContainerConfig, drawerConfig: ISideDrawerConfig<any>) : ComponentRef<any> {
    this.initConfig(containerConfig);

    if (this.drawerItem){
      this.drawerItem.clear();
    }
    
    const componentRef = this.drawerItem.createComponent(drawerConfig.type);
    const component =  componentRef.instance;

    component.data = drawerConfig.data;
    component.close = drawerConfig.close;

    if (!componentRef) {
      throw new Error('Не удалось создать компонент');
    }

    this.drawer.open();
    return componentRef;
  }

  private initConfig(config?: ISideDrawerContainerConfig) {
    this.config = config ? config : { 
      hasBackdrop: 'false',
      autosize: 'false',
      mode: 'over', 
      position: 'end',
      drawerclass: 'modal-sm'
    } as ISideDrawerContainerConfig;
  }

  closeDrawer() {
    this.drawer.close();
  }
}
