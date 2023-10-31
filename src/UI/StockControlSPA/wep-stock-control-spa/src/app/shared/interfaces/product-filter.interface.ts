import { Guid } from "guid-ts";

export interface IProductFilter {
    partyId?: Guid;
    nomenclatureId?: Guid;
    warehouseId?: Guid;
    organizationId?: Guid;
}