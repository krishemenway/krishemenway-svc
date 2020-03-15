declare module "*.png";
declare module "*.jpg";
declare module "*.gif";

interface Dictionary<T> {
	[key: string]: T;
}