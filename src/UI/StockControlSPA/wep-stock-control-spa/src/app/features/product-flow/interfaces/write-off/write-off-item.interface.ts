import { IEntity } from "src/app/shared/interfaces/entity.interface";
import { IProductFlowInfo } from "../product-flow-info.interface";
import { ISendingProduct } from "../sending-product.interface";

export interface IWriteOffItem extends IEntity, ISendingProduct, IProductFlowInfo {
    //** Причина */
    reason?: string;
}