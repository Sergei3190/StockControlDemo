import { Guid } from "guid-ts";

export interface ISelectOrganizationFilterParams  {
    partyId?: Guid;
    warehouseId?: Guid;
    nomenclatureId?: Guid;
}