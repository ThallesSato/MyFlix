type TabType = 'todos' | 'assistidos' | 'nao-assistidos';

interface FilmeTabsProps {
	activeTab: TabType;
	onTabChange: (tab: TabType) => void;
}

export default function FilmeTabs({ activeTab, onTabChange }: FilmeTabsProps) {
	const tabConfig = [
		{ key: 'todos' as TabType, label: 'Todos' },
		{ key: 'assistidos' as TabType, label: 'Assistidos' },
		{ key: 'nao-assistidos' as TabType, label: 'Não Assistidos' }
	];

	return (
		<div className="border-b border-gray-200 mb-6">
			<nav className="flex space-x-3 lg:space-x-10 justify-center">
				{tabConfig.map(tab => (
					<button
						key={tab.key}
						onClick={() => onTabChange(tab.key)}
						className={`py-2 px-1 border-b-2 font-size text-lg ${
							activeTab === tab.key
								? 'border-[#6C757D] text-[#6C757D]'
								: 'border-transparent text-gray-500 hover:text-[#212529] hover:border-[#212529]'
						}`}
					>
						{tab.label}
					</button>
				))}
			</nav>
		</div>
	);
}
