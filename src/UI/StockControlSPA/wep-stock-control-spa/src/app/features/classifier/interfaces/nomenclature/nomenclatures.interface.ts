import { IPaginatedItems } from "src/app/shared/interfaces/paginated-items.interface";
import { INomenclatureItem } from "./nomenclature-item.interface";

export interface INomenclatures extends IPaginatedItems {
    items: INomenclatureItem[];
}