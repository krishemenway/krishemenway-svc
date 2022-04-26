import * as React from "react";
import clsx from "clsx";
import { useMargin, useText, useLayout } from "Common/AppStyles";
import { createUseStyles } from "react-jss";

const LoadingSpinner: React.FC = () => {
	const [loading, layout, text, margin] = [useLoadingStyles(), useLayout(), useText(), useMargin()];

	return (
		<div className={clsx(text.center)} style={{ padding: "72px 0" }}>
			<div className={clsx(margin.verticalDouble)}><LoadingIcon size="48px" color="#FFF" /></div>
			<div>
				<div className={clsx(layout.inlineBlock, margin.bottom)}>Loading</div>

				<div className={clsx(layout.inlineBlock, margin.bottom, text.left, loading.ellipsesContainer)}>
					<div className={clsx(layout.inlineBlock, loading.ellipses)}>&hellip;</div>
				</div>
			</div>
		</div>
	);
};

const useLoadingStyles = createUseStyles(() => ({
	'@keyframes ellipses': {
		"0%": { width: "4px", },
		"32%": { width: "4px", },
		"33%": { width: "8px", },
		"65%": { width: "8px", },
		"66%": { width: "12px", },
		"100%": { width: "12px", },
	},
	ellipsesContainer: {
		width: "1em",
	},
	ellipses: {
		animation: "4s linear 0s infinite normal none running $ellipses",
		overflow: "hidden",
	},
}));

const LoadingIcon: React.FC<{ size: string; color: string; }> = ({ size, color }) => {
	const [loading] = [useLoadingIconStyles()];

	return (
		<svg width={size} height={size} color={color} viewBox="22 22 44 44">
			<circle className={loading.circle} cx="44" cy="44" r="20.2" fill="none" strokeWidth="3.6"></circle>
		</svg>
	);
};

const useLoadingIconStyles = createUseStyles(() => ({
	'@keyframes circleAnimation': {
		"0%": { strokeDasharray: "1px,200px", strokeDashoffset: "0" },
		"50%": { strokeDasharray: "100px,200px", strokeDashoffset: "-15px" },
		"100%": { strokeDasharray: "100px,200px", strokeDashoffset: "-125px" },
	},
	circle: {
		stroke: "currentcolor",
		strokeDasharray: "80px, 200px",
		strokeDashoffset: "0px",
		animation: "1.4s ease-in-out 0s infinite normal none running $circleAnimation",
	},
}));

export default LoadingSpinner;
