import { Guid } from "guid-ts";

export interface ISelectWarehouseFilterParams {
    partyId?: Guid;
    organizationId?: Guid;
    nomenclatureId?: Guid;
}