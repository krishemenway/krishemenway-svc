import * as React from "react";
import ReactDOM = require("react-dom");
import { withStyles, createStyles, Theme, WithStyles } from "@material-ui/core/styles";

const body = document.getElementsByTagName("body")[0];
const modalRoot = document.createElement('div');
body.appendChild(modalRoot);

interface ModalProps extends WithStyles<typeof styles> {
	Open: boolean;
	className?: string;
}

class Modal extends React.Component<ModalProps, {}> {
	constructor(props: ModalProps) {
		super(props);
		modalRoot.className = props.classes.modalOverlay;
		this.element = document.createElement("div");
	}

	public render() {
		this.setModalElements();
		return ReactDOM.createPortal(this.props.children, this.element);
	}

	public componentWillReceiveProps() {
		this.setModalElements();
	}

	public componentWillMount() {
		modalRoot.appendChild(this.element);
	}

	public componentWillUnmount() {
		modalRoot.removeChild(this.element);
	}

	private setModalElements() {
		this.element.className = `${this.props.classes.modal} ${this.props.className}`;
		body.className = this.props.Open ? this.props.classes.isOpen : "";
	}

	private element: HTMLDivElement;
}

const styles = createStyles({
	modalOverlay: {
		position: "fixed",
		top: 0,
		bottom: 0,
		left: 0,
		right: 0,
		zIndex: 10,
		display: "none",

		"$isOpen &": {
			display: "block",
		},
	},
	modal: {
		position: "fixed",
		top: "50px",
		left: "50%",
		display: "none",

		"$isOpen &": {
			display: "block",
		},
	},
	isOpen: {
		overflow: "hidden",
	},
});

export default withStyles(styles)(Modal);