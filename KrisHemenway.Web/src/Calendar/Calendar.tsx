import * as React from "react";
import * as reactDom from "react-dom";
import * as moment from "moment";
import {  } from "@residualeffect/reactor";
import { Episode } from "Episodes/Episode";
import CalendarMonth from "./CalendarMonth";
import MonthNavigation from "./MonthNavigation";
import DownloadLogin from "./DownloadLogin";
import ListOf from "Common/ListOf";
import EpisodeName from "Episodes/EpisodeName";
import {default as AppBackground} from "Common/AppBackground.png";
import { makeStyles } from "@material-ui/core/styles";
import Loading from "Common/Loading";
import { CalendarService } from "Calendar/CalendarService";
import { useObservable } from "Common/UseObservable";
const EpisodeList = ListOf<Episode>();

export const Calendar: React.FC = () => {
	const classes = useCalendarStyles();
	const currentMonth = useObservable(CalendarService.Instance.ViewingMonth);
	const episodesByDayKey = useObservable(CalendarService.Instance.FindOrCreateEpisodesForMonth(currentMonth));
	const isLoading = useObservable(CalendarService.Instance.FindOrCreateCurrentMonthsIsLoading(currentMonth));

	return (
		<div className={classes.calendarApp}>
			<div className={classes.widthWrapper}>
				<MonthNavigation
					CurrentMonth={currentMonth}
					OnChangeMonth={(month) => CalendarService.Instance.ViewingMonth.Value = month}
				/>

				<Loading
					IsLoading={isLoading}
					RenderChildren={() =>
						<CalendarMonth
							Month={currentMonth}
							DayRenderers={[
								{ type: "episodes", render: (day) => <CalendarEpisodeList Date={day} EpisodesByDayKey={episodesByDayKey} /> },
							]}
						/>
					}
				/>

				<DownloadLogin
					OnAuthenticated={() => { }}
					IsAuthenticated={false}
				/>
			</div>
		</div>
	);
}

const useCalendarStyles = makeStyles({
	calendarApp: { },
	widthWrapper: {
		margin: "0 auto",
		maxWidth: "900px",
		padding: "0 10px 100px 10px",
		background: "rgba(0,0,0,.2)",
		textAlign: "center",
	},
});

interface CalendarEpisodeListProps {
	EpisodesByDayKey: Dictionary<Episode[]>;
	Date: moment.Moment;
}

const CalendarEpisodeList: React.FC<CalendarEpisodeListProps> = (props) => {
	const classes = useCalendarEpisodeListStyles();

	return (
		<EpisodeList
			className={classes.episodes}
			key={`episodes-${props.Date.format("YYYY-MM-DD")}`}
			items={props.EpisodesByDayKey[props.Date.format("YYYY-MM-DD")]}
			renderItem={(episode) => <EpisodeName className={classes.episodeName} Episode={episode} ShowDownload={false} />}
		/>
	);
};

const useCalendarEpisodeListStyles = makeStyles({
	episodes: {
		flexBasis: "auto",
		zIndex: 1,
	},
	episodeName: {
		margin: "5px 0",
		textAlign: "left",

		"&:first-child": {
			marginTop: "0",
		},

		"&:last-child": {
			marginBottom: "0",
		},
	},
});

(window as any).initialize = () => {
	reactDom.render(<Calendar />, document.getElementById("app"));

	let body = document.getElementsByTagName("body")[0];
	body.style.background = `url('${AppBackground}') #010101`;
	body.style.height = "100%";
}
