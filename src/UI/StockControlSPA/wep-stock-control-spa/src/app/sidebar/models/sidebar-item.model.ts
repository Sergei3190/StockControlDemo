export class SideBarItem {
    label: string;
    href?: string;
    iconClass?: string;
    disabled?: boolean;

    constructor (label: string, href?: string, iconClass?: string, disabled?: boolean){
        this.label = label;
        this.href = href;
        this.iconClass = iconClass;
        this.disabled = disabled;
    }
}