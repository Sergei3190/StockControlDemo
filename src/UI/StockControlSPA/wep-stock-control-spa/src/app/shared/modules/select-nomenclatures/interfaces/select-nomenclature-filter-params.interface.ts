import { Guid } from "guid-ts";

export interface ISelectNomenclatureFilterParams {
    partyId?: Guid;
    warehouseId?: Guid;
    organizationId?: Guid;
}