import { IEntity } from "src/app/shared/interfaces/entity.interface";
import { IProduct } from "src/app/shared/interfaces/product.interface";
import { IProductFlowInfo } from "../product-flow-info.interface";

export interface IReceiptItem extends IEntity, IProduct, IProductFlowInfo {

}