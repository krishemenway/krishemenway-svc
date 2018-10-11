import * as React from "react";
import * as reactDom from "react-dom";
import { ProjectLink } from "./ProjectLink";

export class App extends React.Component<{}, {}> {
	public render() {
		return (
			<div className="home-app">
				<div className="projects">
					<h1 className="font-28 gray-c8 text-center to-upper text-inset bold margin-vertical padding-vertical">
						Projects
					</h1>

					<ProjectLink
						Title="TV Show Release Calendar"
						Description="complete calendar listing of all the TV Shows being tracked"
						Location="https://www.krishemenway.com/calendar.html" />

					<ProjectLink
						Title="Sloshy Dosh Man Killing Floor 2 Stats"
						Description="tracking the stats of local killing floor 2 server"
						Location="https://www.sloshydoshman.com" />

					<ProjectLink
						Title="Automated Typing Center for America"
						Description="what is happening right now."
						Location="https://www.krishemenway.com/im-busy" />

					<ProjectLink
						Title="Supermarket Ninja (mobile)"
						Description="swipin food away at the cash register!"
						Location="https://www.krishemenway.com/supermarketninja" />
				</div>
			</div>
		)
	}
}

reactDom.render(<App />, document.getElementById('app'));
