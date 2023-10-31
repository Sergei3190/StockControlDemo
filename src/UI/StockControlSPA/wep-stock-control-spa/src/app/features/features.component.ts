import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
    selector: 'app-features',
    template: '<router-outlet></router-outlet>',
    styleUrls: ['features.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class FeaturesComponent implements OnInit {
    constructor() {}

    async ngOnInit(): Promise<void> {}
}
