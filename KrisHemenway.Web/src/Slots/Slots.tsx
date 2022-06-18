import * as React from "react";
import * as reactDom from "react-dom";
import clsx from "clsx";
import { useObservable } from "@residualeffect/rereactor";
import { createUseStyles } from "react-jss";
import { SlotMachine } from "./SlotMachine";
import { SlotReel } from "./SlotReel";
import { Loading } from "@krishemenway/react-loading-component";
import { SlotWinningPattern } from "./SlotWinningPattern";
import LockedIcon from "Common/LockedIcon";
import UnlockedIcon from "Common/UnlockedIcon";
import {default as AppBackground} from "Common/AppBackground.png";
import {default as Banana} from "Slots/Banana.png";
import {default as Cherry} from "Slots/Cherry.png";
import {default as LuckySeven} from "Slots/LuckySeven.png";
import {default as Lemon} from "Slots/Lemon.png";

const Slots : React.FC<{}> = () => {
	const classes = useStyles();
	const machine = React.useMemo(() => new SlotMachine(), []);

	return (
		<div className={classes.app}>
			<Scoreboard slotMachine={machine} />
			<SlotReels slotMachine={machine} />

			<div className={classes.actionRow}>
				<WinningPatterns slotMachine={machine} />

				<div>
					<SpinButton slotMachine={machine} />
					<SpinResultMessage slotMachine={machine} />
				</div>
			</div>
		</div>
	);
};

const SlotReels: React.FC<{ slotMachine: SlotMachine }> = (props) => {
	const classes = useStyles();
	const isPreLockSpin = useObservable(props.slotMachine.PreLockSpin);

	return (
		<div className={classes.reels}>
			{props.slotMachine.Reels.map((reel, index) => (
				<SlotReelComponent
					reel={reel}
					reelIndex={index}
					machine={props.slotMachine}
					defaultSymbol={props.slotMachine.AllSymbols[0]}
					canChangeLocks={!isPreLockSpin}
				/>
			))}
		</div>
	);
};

const SlotReelComponent: React.FC<{ machine: SlotMachine; reel: SlotReel; defaultSymbol: string; canChangeLocks: boolean; reelIndex: number }> = (props) => {
	const classes = useStyles();
	return (
		<div className={classes.reelWrapper}>
			<div className={classes.reel}>
				<Loading
					receivers={[props.reel.CurrentSymbol]}
					whenNotStarted={<SlotSymbolComponent symbol={props.machine.AllSymbols[0]} />}
					whenError={(errors) => <>Error</>}
					whenLoading={<SpinningSlotSymbol slotMachine={props.machine} animationDelay={props.reelIndex} />}
					whenReceived={(symbol) => <SlotSymbolComponent symbol={symbol} />}
				/>
			</div>

			<LockButton reel={props.reel} canChangeLocks={props.canChangeLocks} />
		</div>
	);
};

const WinningPatterns: React.FC<{ slotMachine: SlotMachine }> = (props) => {
	const classes = useStyles();
	const currentWinningPattern = useObservable(props.slotMachine.MostRecentWonPattern);

	return (
		<div className={classes.winningPatterns}>
			{props.slotMachine.WinningPatterns.map((wp, index) => <WinningPattern key={index} pattern={wp} selected={currentWinningPattern === wp} />)}
		</div>
	);
};

const WinningPattern: React.FC<{ pattern: SlotWinningPattern; selected: boolean; }> = (props) => {
	const classes = useStyles();
	return (
		<div className={clsx(classes.winningPattern, props.selected && "selected")}>
			<div className="earnPoints">{props.pattern.EarnsPoints}</div>
			<div>
				{Object.keys(props.pattern.RequiredSymbols).map((symbolKey, index) => (
					<>{Array.from({ length: props.pattern.RequiredSymbols[symbolKey] } , () => <SlotSymbolComponent symbol={symbolKey} size="24px" />)}</>
				))}
			</div>
		</div>
	);
};

const LockButton: React.FC<{ reel: SlotReel; canChangeLocks: boolean; }> = (props) => {
	const classes = useStyles();
	const isLocked = useObservable(props.reel.IsLocked);
	const reelIsBusy = useObservable(props.reel.CurrentSymbol.IsBusy);

	return (
		<button className={clsx(classes.lockButton, isLocked && "locked")} type="button" disabled={!props.canChangeLocks || reelIsBusy} onClick={() => { props.reel.ToggleLock(); }}>
			{isLocked ? <LockedIcon /> : <UnlockedIcon />}
		</button>
	);
};

const SpinButton: React.FC<{ slotMachine: SlotMachine }> = (props) => {
	const classes = useStyles();

	return (
		<Loading
			receivers={[
				props.slotMachine.Reels[0].CurrentSymbol,
				props.slotMachine.Reels[1].CurrentSymbol,
				props.slotMachine.Reels[2].CurrentSymbol,
			]}
			whenNotStarted={<button className={classes.spinButton} type="button" onClick={() => { props.slotMachine.Spin(); }}>Spin</button>}
			whenReceived={(symbol) => <button className={classes.spinButton} type="button" onClick={() => { props.slotMachine.Spin(); }}>Spin Again</button>}
			whenLoading={<button className={classes.spinButton} type="button" disabled>Spinning</button>}
			whenError={(errors) => <>Error</>}
		/>
	);
};

const SlotSymbolComponent: React.FC<{ symbol: string; size?: string; }> = (props) => {
	const classes = useStyles();
	return <div className={clsx(classes.slotSymbol, props.symbol)} style={{ height: props.size, width: props.size }} ></div>;
};

const SpinningSlotSymbol: React.FC<{ slotMachine: SlotMachine; animationDelay: number }> = (props) => {
	const classes = useStyles();

	return (
		<div className={classes.symbolSpinner}>
			<div style={{ height: 80 * props.slotMachine.AllSymbols.length, animationDelay: `.${props.animationDelay}s` }}>
				{props.slotMachine.AllSymbols.map((s) => <SlotSymbolComponent key={`1-${s}`} symbol={s} />)}
				<SlotSymbolComponent symbol={props.slotMachine.AllSymbols[0]} />
			</div>
		</div>
	);
};

const Scoreboard: React.FC<{ slotMachine: SlotMachine }> = (props) => {
	const classes = useStyles();
	const score = useObservable(props.slotMachine.Score);

	return (
		<div>
			<div className={classes.scoreTitle}>Score</div>
			<div className={classes.scoreValue}>{score}</div>
		</div>
	);
};

const SpinResultMessage: React.FC<{ slotMachine: SlotMachine }> = (props) => {
	const classes = useStyles();
	const spinWinFlag = useObservable(props.slotMachine.SpinWinFlag);
	const mostRecentPattern = useObservable(props.slotMachine.MostRecentWonPattern);

	if (spinWinFlag === null) {
		return <></>;
	}

	return (
		<div className={clsx(classes.winMessage)}>
			{spinWinFlag === true && (<><div>YOU</div><div>WIN</div><div>+{mostRecentPattern?.EarnsPoints}</div></>)}
			{spinWinFlag === false && (<><div>YOU</div><div>LOSE</div></>)}
		</div>
	);
};

const useStyles = createUseStyles({
	"@keyframes spinner": {
		from: {top: "0px"},
		to: {top: "-320px"},
	},
	actionRow: {
		display: "flex",
		flexDirection: "row",
		gap: "16px",
		width: "316px",
		margin: "0 auto",

		"& > *:last-child": {
			flexGrow: 1,
		},
	},
	app: {
		background: "#080808",
		color: "#E8E8E8",
		paddingTop: "16px",
		width: "100%",
		maxWidth: "475px",
		margin: "8px auto 0 auto",
		boxShadow: "0 3px 4px 0 rgba(0,0,0,.5)",
		paddingBottom: "16px",
	},
	lockButton: {
		fontSize: "26px",
		padding: "8px 0",

		"&.locked": {
			color: "#00F",
		},
		"&:hover": {
			color: "#00F",
			cursor: "pointer",
		},
		"&:disabled": {
			color: "#CCC",
			cursor: "not-allowed",
		},
		"&:hover:disabled": {
			color: "#CCC",
			cursor: "not-allowed",
		},
	},
	winMessage: {
		background: "#101010",
		width: "100%",
		margin: "16px auto",
		padding: "8px 24px",
		lineHeight: "30px",
		fontSize: "24px",
		textAlign: "center",
	},
	winningPatterns: {
		display: "flex",
		flexDirection: "column",
		minWidth: "170px",
		padding: "8px",
		background: "#101010",
		borderRadius: "4px",
	},
	winningPattern: {
		display: "flex",
		flexDirection: "row",
		flexWrap: "nowrap",
		gap: "8px",
		borderBottom: "1px solid #606060",
		padding: "8px",

		"&:last-child": {
			borderBottom: "none",
		},

		"&.selected": {
			background: "#404040",
		},
		"& .earnPoints": {
			fontSize: "24px",
			textAlign: "center",
			width: "50px",
		},
	},
	reels: {
		display: "flex",
		flexDirection: "row",
		flexWrap: "nowrap",
		gap: "8px",
		justifyContent: "center",
		marginBottom: "16px",
	},
	reelWrapper: {
		display: "flex",
		flexDirection: "column",
	},
	reel: {
		display: "flex",
		width: "100px",
		height: "100px",
		border: "1px solid #C0C0C0",
	},
	slotSymbol: {
		margin: "auto",
		width: "80px",
		height: "80px",
		textAlign: "center",
		backgroundRepeat: "no-repeat",
		backgroundSize: "cover",
		display: "inline-block",

		"&.LuckySeven": {
			backgroundImage: `url('${LuckySeven}')`,
		},

		"&.Banana": {
			backgroundImage: `url('${Banana}')`,
		},

		"&.Cherry": {
			backgroundImage: `url('${Cherry}')`,
		},

		"&.Lemon": {
			backgroundImage: `url('${Lemon}')`,
		},
	},
	spinButton: {
		display: "block",
		padding: "16px 0",
		cursor: "pointer",
		border: "1px solid #C0C0C0",
		backgroundColor: "#101010",
		width: "100%",

		"&:hover": {
			backgroundColor: "#505050",
		},
		"&:hover:disabled": {
			backgroundColor: "#101010",
		},
	},
	scoreTitle: {
		fontSize: "24px",
		letterSpacing: "8px",
		textTransform: "uppercase",
		textAlign: "center",
	},
	scoreValue: {
		fontSize: "48px",
		fontWeight: "bold",
		letterSpacing: "8px",
		textAlign: "center",
		margin: "4px 0 24px 0",
	},
	symbolSpinner: {
		margin: "auto",
		overflow: "hidden",
		width: "80px",
		height: "80px",

		"& > *": {
			top: "0px",
			animation: "$spinner linear infinite .5s",
			position: "relative",
		},
	},
});

(window as any).initialize = (element: HTMLElement) => {
	reactDom.render(<Slots />, element);
	document.getElementsByTagName("body")[0].style.background = `url('${AppBackground}') #010101`;
}
