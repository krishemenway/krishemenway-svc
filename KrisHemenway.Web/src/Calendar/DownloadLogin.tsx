import * as React from "react";
import { Modal } from "../Common/Modal";

interface DownloadLoginParams {
	OnAuthenticated: () => void;
}

interface DownloadLoginState {
	LoginOpen: boolean;
	ErrorText: string;
}

export class DownloadLogin extends React.Component<DownloadLoginParams, DownloadLoginState> {
	constructor(props: DownloadLoginParams) {
		super(props);

		this.state = {
			LoginOpen: false,
			ErrorText: "",
		};
	}

	public render() {
		return (
			<div className="text-center margin-vertical">
				<a className="download-login-link" href="#" onClick={(() => this.setState({LoginOpen: true}))}>Login</a>

				<Modal className="download-login-form padding-horizontal padding-vertical" Open={this.state.LoginOpen}>
					<form onSubmit={() => this.onLoginSubmitted()} className="text-center">
						<div className="download-form-header font-24 to-upper margin-vertical bold">Password</div>

						<div className="margin-vertical">
							<input className="font-16 download-login-field" type="password" ref={(element) => this.passwordElement = element} />
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
				this.setState({ErrorText: response.ErrorMessage});
			}
		 })
		 .fail((response) => {
			this.setState({ErrorText: "Something went wrong with this request"});
		 });
	}

	private passwordElement: HTMLInputElement;
}
