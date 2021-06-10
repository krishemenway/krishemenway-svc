const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const CopyPlugin = require("copy-webpack-plugin");
const path = require("path");

module.exports = {
	entry: {
		app: "./src/Home/App.tsx",
		arts: "./src/Arts/App.tsx",
		missingepisodes: "./src/MissingEpisodes/MissingEpisodes.tsx",
		calendar: "./src/Calendar/Calendar.tsx"
	},

	output: {
		filename: "[name].js",
		path: __dirname + "/dist"
	},

	resolve: {
		extensions: [".ts", ".tsx", ".js", ".json"],
		modules: [
			path.resolve(__dirname, "src"),
			"node_modules"
		]
	},

	module: {
		rules: [
			{ test: /\.(png|jpg|gif)$/, use: [{ loader: "file-loader", options: {} }], },
			{ test: /\.tsx?$/, loader: "ts-loader" },
		]
	},

	plugins: [
		new CleanWebpackPlugin({ cleanStaleWebpackAssets: false }),
		new CopyPlugin({
			patterns: [
				{ from: "./src/favicon.ico", to: "." },
				{ from: "./src/**/*.html", to: "[name][ext]" },
				{ from: "./misc_projects", to: "." },
			]
		}),
	],

	externals: {
		"react": "React",
		"react-dom": "ReactDOM",
		"moment": "moment",
	},

	watchOptions: {
		poll: 5000,
	}
};