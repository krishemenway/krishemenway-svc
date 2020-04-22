import * as React from "react";
import { CircularProgress } from "@material-ui/core";

interface LoadingProps {
	IsLoading: boolean;
	Render: () => JSX.Element|JSX.Element[];
}

const Loading : React.FC<LoadingProps> = (props) => {
	if (props.IsLoading) {
		return <CircularProgress style={{margin: "24px 0"}} />;
	}

	return <>{props.Render()}</>;
};

export default Loading;