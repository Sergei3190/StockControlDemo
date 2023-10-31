import { Guid } from "guid-ts";

export interface IBulkDeleteSuccessMessage {
    message?: string; 
    ids?: Guid[];
}