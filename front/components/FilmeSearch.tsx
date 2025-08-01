import React, { useState } from "react";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faMagnifyingGlass} from "@fortawesome/free-solid-svg-icons";
interface FilmeSearchProps {
	onSearch: (term: string) => void;
}

export default function FilmeSearch({ onSearch }: FilmeSearchProps) {
	const [search, setSearch] = useState("");

	const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const value = e.target.value;
		setSearch(value);
		onSearch(value);
	};

	return (
		<div className="flex items-center border rounded px-3 py-2 mb-6 max-h-9  bg-[#F8F9FA] shadow-sm">
			<FontAwesomeIcon icon={faMagnifyingGlass} className="max-w-4"/>
			<input
				type="text"
				value={search}
				onChange={handleChange}
				placeholder="Buscar filme..."
				className="outline-none flex-1 text-sm"
			/>
		</div>
	);
}
