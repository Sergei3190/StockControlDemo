import { Guid } from "guid-ts";
import { IFilter } from "src/app/shared/interfaces/filter.interface";

export interface ISelectWarehouseFilter extends IFilter {
    partyId?: Guid;
    organizationId?: Guid;
    nomenclatureId?: Guid;
}