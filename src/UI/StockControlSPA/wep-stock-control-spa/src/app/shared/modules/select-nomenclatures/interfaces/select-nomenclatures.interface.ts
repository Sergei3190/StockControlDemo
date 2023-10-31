import { INamedEntity } from "src/app/shared/interfaces/named-entity.interface";
import { IPaginatedItems } from "src/app/shared/interfaces/paginated-items.interface";

export interface ISelectNomenclatures extends IPaginatedItems {
    items: INamedEntity[];
}