import { Guid } from "guid-ts";

export interface IProductFlowInfoMessage {
    productFlowId: Guid;
    number: string;
}