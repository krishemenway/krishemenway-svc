import * as React from "react";
import { Modal } from "../Common/Modal";
import { withStyles, createStyles, Theme, WithStyles } from "@material-ui/core/styles";

interface DownloadLoginParams extends WithStyles<typeof styles> {
	OnAuthenticated: () => void;
	IsAuthenticated: boolean;
}

interface DownloadLoginState {
	LoginOpen: boolean;
	ErrorText: string;
}

export class DownloadLogin extends React.Component<DownloadLoginParams, DownloadLoginState> {
	constructor(props: DownloadLoginParams) {
		super(props);

		this.passwordElement = null;

		this.state = {
			LoginOpen: false,
			ErrorText: "",
		};
	}

	public render() {
		if (this.props.IsAuthenticated) {
			return "";
		}

		return (
			<div className={this.props.classes.root}>
				<a className={this.props.classes.downloadLoginLink} href="#" onClick={(() => this.setState({LoginOpen: true}))}>Login</a>

				<Modal className={this.props.classes.downloadLoginForm} Open={this.state.LoginOpen}>
					<form onSubmit={() => this.onLoginSubmitted()} style={{textAlign: "center"}}>
						<div className="download-form-header font-24 to-upper margin-vertical bold">Password</div>

						<div className="margin-vertical">
							<input tabIndex={0} className="font-16 download-login-field" type="password" ref={(element) => this.passwordElement = element} />
						</div>

						{!!this.state.ErrorText ? <div className="font-16 text-center download-login-error margin-vertical">{this.state.ErrorText}</div> : ""}

						<div className="download-login-buttons text-right margin-vertical">
							<button className="font-18 margin-horizontal" type="button" onClick={() => this.setState({LoginOpen: false})}>Cancel</button>
							<button className="font-18 margin-horizontal" type="submit">Login</button>
						</div>
					</form>
				</Modal>
			</div>
		);
	}

	private onLoginSubmitted() {
		this.setState({ErrorText: ""});

		if (this.passwordElement === null) {
			throw "Password element was not referenced";
		}

		$.ajax({
			url: "/api/tvshows/episodes/authenticate",
			method: "POST",
			data: JSON.stringify({Password: this.passwordElement.value}),
			contentType: "application/json; charset=utf-8",
			dataType: "json",
		})
		 .done((response) => {
			if (response.Success) {
				this.setState({LoginOpen: false});
				this.props.OnAuthenticated();
			} else {
				if (this.passwordElement === null) {
					throw "Password element was not referenced";
				}

				this.setState({ErrorText: response.ErrorMessage});
				this.passwordElement.focus();
			}
		 })
		 .fail((response) => {
			this.setState({ErrorText: "Something went wrong with this request"});
		 });
	}

	private passwordElement: HTMLInputElement | null;
}

const styles = (_: Theme) => createStyles({
	root: {
		textAlign: "center",
		margin: "10px 0",
	},
	downloadLoginLink: {

	},
	downloadLoginForm: {
		padding: "10px",
	},
});

export default withStyles(styles)(DownloadLogin);
