/** Интерфейс для настройки контейнера и его выдвижного ящика */
export interface ISideDrawerContainerConfig {
    hasBackdrop: 'true' | 'false';
    autosize: 'true' | 'false';
    mode: 'side' | 'over' | 'push';
    position: 'start' | 'end';
    drawerclass: 'modal-lg' | 'modal-sm';
}