import { Guid } from "guid-ts";
import { IFilter } from "src/app/shared/interfaces/filter.interface";

export interface ISelectNomenclatureFilter extends IFilter {
    partyId?: Guid;
    warehouseId?: Guid;
    organizationId?: Guid;
}