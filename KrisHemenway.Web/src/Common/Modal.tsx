import * as React from "react";
import * as ReactDOM from "react-dom";
import { createUseStyles } from "react-jss";

const body = document.getElementsByTagName("body")[0];
const modalRoot = document.createElement('div');
body.appendChild(modalRoot);

interface ModalProps {
	Open: boolean;
	className?: string;
	onClosed: () => void;
}

const Modal: React.FC<ModalProps> = (props) => {
	const classes = useStyles();
	const [element, setElement] = React.useState(document.createElement("div"));
	React.useEffect(() => {
		if (props.Open) {
			modalRoot.appendChild(element);
		} else if (modalRoot.contains(element)) {
			modalRoot.removeChild(element);
		}
		modalRoot.className = classes.modalOverlay;
		modalRoot.onclick = (evt) => { if (evt.target == modalRoot) { props.onClosed(); } };
		element.className = `${classes.modal} ${props.className ?? ""}`;
		body.className = modalRoot.hasChildNodes() ? classes.isOpen : "";
	}, [ props.Open ]);
	return ReactDOM.createPortal(props.children, element);
};

const useStyles = createUseStyles({
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
		position: "absolute",
		top: "50px",
		left: "50%",
		transform: "translateX(-50%)",
		display: "none",

		"$isOpen &": {
			display: "block",
		},
	},
	isOpen: {
		overflow: "hidden",
	},
});

export default Modal;