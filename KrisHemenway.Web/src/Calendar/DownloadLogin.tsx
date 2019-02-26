import * as React from "react";
import Modal from "../Common/Modal";
import Text from "../Common/Text";
import { withStyles, createStyles, Theme, WithStyles } from "@material-ui/core/styles";

interface DownloadLoginParams extends WithStyles<typeof styles> {
	OnAuthenticated: () => void;
	IsAuthenticated: boolean;
}

interface DownloadLoginState {
	LoginOpen: boolean;
	Password: string;
	ErrorText: string;
}

export class DownloadLogin extends React.Component<DownloadLoginParams, DownloadLoginState> {
	constructor(props: DownloadLoginParams) {
		super(props);

		this.state = {
			LoginOpen: false,
			ErrorText: "",
			Password: "",
		};
	}

	public render() {
		if (this.props.IsAuthenticated) {
			return "";
		}

		return (
			<div className={this.props.classes.root}>
				<a className={this.props.classes.loginLink} href="#" onClick={(() => this.setState({LoginOpen: true}))}>Login</a>

				<Modal className={this.props.classes.loginForm} Open={this.state.LoginOpen}>
					<form onSubmit={() => this.onLoginSubmitted()}>
						<Text className={this.props.classes.loginHeader} Text="Password" />

						<input tabIndex={0} className={this.props.classes.loginPassword} type="password" onChange={(evt) => this.setState({ Password: evt.target.value })} />

						{!!this.state.ErrorText && <Text className={this.props.classes.error} Text={this.state.ErrorText} />}

						<div className={this.props.classes.loginButtons}>
							<button className={this.props.classes.loginButton} type="button" onClick={() => this.setState({LoginOpen: false})}>Cancel</button>
							<button className={this.props.classes.loginButton} type="submit">Login</button>
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
			data: JSON.stringify({Password: this.state.Password}),
			contentType: "application/json; charset=utf-8",
			dataType: "json",
		})
		 .done((response) => {
			if (response.Success) {
				this.setState({LoginOpen: false, Password: ""});
				this.props.OnAuthenticated();
			} else {
				this.setState({ErrorText: response.ErrorMessage, Password: ""});
			}
		 })
		 .fail(() => {
			this.setState({ErrorText: "Something went wrong with this request", Password: ""});
		 });
	}
}

const styles = (_: Theme) => createStyles({
	root: {
		textAlign: "center",
		margin: "10px 0",
	},
	loginLink: {
		textDecoration: "none",
		color: "#F0F0F0",
	},
	loginForm: {
		padding: "20px",
		width: "400px",
		marginLeft: "-200px",
		background: "#F0F0F0",
		border: "1px solid #C0C0C0",
	},
	loginPassword: {
		fontSize: "16px",
		border: "1px solid #202020",
		padding: "3px 8px",
		marginBottom: "20px",
	},
	loginButtons: {
		textAlign: "right",
	},
	loginButton: {
		fontSize: "16px",
		color: "#101010",
		padding: "3px 8px",
		marginLeft: "10px",
		background: "#E0E0E0",
		border: "1px solid #C0C0C0",
		cursor: "pointer",

		"&:hover": {
			background: "#D0D0D0",
		},
	},
	loginHeader: {
		fontSize: "18px",
		display: "block",
		color: "#101010",
		borderBottom: "1px solid #C0C0C0",
		padding: "0 20px 20px 20px",
		margin: "0 -20px 20px -20px",
	},
	error: {
		fontSize: "16px",
		textAlign: "center",
	},
});

export default withStyles(styles)(DownloadLogin);
