import { Guid } from "guid-ts";
import { IFilter } from "src/app/shared/interfaces/filter.interface";

export interface ISelectPartyFilter extends IFilter {
    nomenclatureId?: Guid;
    warehouseId?: Guid;
    organizationId?: Guid;
}