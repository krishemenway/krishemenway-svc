import * as React from 'react';
import * as reactDom from 'react-dom';

export class App extends React.Component<{}, {}>
{
	public render() {
		return (
			<div className="home-app">
				<div className="projects">
					<h1 className="font-28 gray-c8 text-center to-upper text-inset bold margin-vertical padding-vertical">Projects</h1>

					<a className="project-item padding-vertical padding-horizontal" href="https://www.krishemenway.com/calendar.html" title="TV Show Calendar">
						<div className="font-18">TV Show Release Calendar</div>
						<div className="font-12 no-hover-line to-lower">complete calendar listing of all the TV Shows being tracked</div>
					</a>

					<a className="project-item padding-vertical padding-horizontal" href="https://www.sloshydoshman.com" title="Sloshy Dosh Man KF2 Stats">
						<div className="font-18">Sloshy Dosh Man Killing Floor 2 Stats</div>
						<div className="font-12 no-hover-line to-lower">tracking the stats of local killing floor 2 server</div>
					</a>

					<a className="project-item padding-vertical padding-horizontal" href="https://www.krishemenway.com/im-busy" title="I'm Busy">
						<div className="font-18">Automated Typing Center for America</div>
						<div className="font-12 no-hover-line to-lower">what is happening right now.</div>
					</a>

					<a className="project-item padding-vertical padding-horizontal" href="https://www.krishemenway.com/supermarketninja" title="Supermarket Ninja (mobile)">
						<div className="font-18">Supermarket Ninja (mobile)</div>
						<div className="font-12 no-hover-line to-lower">swipin food away at the cash register!</div>
					</a>
				</div>
			</div>
		)
	}
}

reactDom.render(<App />, document.getElementById('app'));