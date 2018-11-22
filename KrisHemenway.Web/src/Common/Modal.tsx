import * as React from "react";
import ReactDOM = require("react-dom");

const body = document.getElementsByTagName("body")[0];
const modalRoot = document.createElement('div');
modalRoot.className = "modal-root";
body.appendChild(modalRoot);

interface ModalProps {
	Open: boolean;
	className: string;
}

export class Modal extends React.Component<ModalProps, {}> {
	constructor(props: ModalProps) {
		super(props);
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
		this.element.className = `modal ${this.props.className}`;
		this.element.style.display = this.props.Open ? "block": "none";
		body.className = this.props.Open ? "is-open" : "";
	}

	private element: HTMLDivElement;
}
