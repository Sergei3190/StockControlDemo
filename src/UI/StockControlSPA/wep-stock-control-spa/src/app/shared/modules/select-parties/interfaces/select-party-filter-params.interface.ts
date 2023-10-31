import { Guid } from "guid-ts";

export interface ISelectPartyFilterParams {
    nomenclatureId?: Guid;
    warehouseId?: Guid;
    organizationId?: Guid;
}