import { ILoadedDataItem } from "../loaded-data/loaded-data-item.interface";

export interface IPersonDocumentItem extends ILoadedDataItem{
    //** Признак отображения действий с документом */
    isHover: boolean;
}