import * as React from "react";

export interface ProjectLinkParams {
	Title: string;
	Location: string;
	Description: string;
}

export class ProjectLink extends React.Component<ProjectLinkParams, {}> {
	public render() {
		return (
			<a className="project-item padding-vertical padding-horizontal" href={this.props.Location} title={this.props.Title}>
				<div className="font-18">
					{this.props.Title}
				</div>

				<div className="font-12 no-hover-line to-lower">
					{this.props.Description}
				</div>
			</a>
		)
	}
}
