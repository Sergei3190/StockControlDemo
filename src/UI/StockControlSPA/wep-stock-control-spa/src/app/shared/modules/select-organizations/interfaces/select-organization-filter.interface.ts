import { Guid } from "guid-ts";
import { IFilter } from "src/app/shared/interfaces/filter.interface";

export interface ISelectOrganizationFilter extends IFilter {
    partyId?: Guid;
    warehouseId?: Guid;
    nomenclatureId?: Guid;
}