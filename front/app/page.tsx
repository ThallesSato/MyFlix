'use client';

import {useState} from "react";
import FilmeModal from "@/components/FilmeModal";
import FilmeList from "@/components/FilmeList";
import FilmeTabs from "@/components/FilmeTabs";
import FilmeSearch from "@/components/FilmeSearch";
import {useFilme} from "@/hooks/useFilme";
import {FilmeDto} from "@/types/FilmeDto";
import {Filme} from "@/types/Filme";
import Alert from "@/components/Alert";

type TabType = 'todos' | 'assistidos' | 'nao-assistidos';
type AlertState = { isOpen: boolean; message: string; type: "success" | "error"; }
type ModalState = { isOpen: boolean; editData: Filme | null; }

export default function Home() {
  const [activeTab, setActiveTab] = useState<TabType>('todos');
  const [searchTerm, setSearchTerm] = useState("");
  const [alert, setAlert] = useState<AlertState>({
    isOpen: false, message: "", type: "success"
  });
  const [modalState, setModalState] = useState<ModalState>({
    isOpen: false, editData: null
  });

  const showAlert = (message: string, type: "success" | "error") => {
    setAlert({
      isOpen: true, message, type
    });
  };
  const {filmes, addFilme, updateFilme, deleteFilme, rateFilme} = useFilme(showAlert);


  const handleSave = async (filmeDto: FilmeDto) => {
    if (modalState.editData) {
      await updateFilme(modalState.editData.id, filmeDto);
    } else {
      await addFilme(filmeDto);
    }
    setModalState({isOpen: false, editData: null});
  };

  const filteredFilmes = filmes.filter(f => {
    const matchTab = activeTab === 'assistidos' ? f.status : activeTab === 'nao-assistidos' ? !f.status : true;

    const matchSearch = f.titulo.toLowerCase().includes(searchTerm.toLowerCase());

    return matchTab && matchSearch;
  });

  return (<main className="px-6 max-w-5xl mx-auto">
    <div className="w-full flex my-2 justify-center">
      <button
        onClick={() => setModalState({isOpen: true, editData: null})}
        className="bg-[#212529] hover:bg-[#343A40] text-white px-4 py-2 rounded"
      >
        Adicionar Filme
      </button>
    </div>

    <FilmeTabs activeTab={activeTab} onTabChange={setActiveTab}/>

    <FilmeSearch onSearch={setSearchTerm}/>

    <FilmeList
      filmes={filteredFilmes}
      onEdit={(f) => setModalState({isOpen: true, editData: f})}
      onDelete={deleteFilme}
      onRating={rateFilme}
    />

    <FilmeModal
      isOpen={modalState.isOpen}
      onClose={() => setModalState({isOpen: false, editData: null})}
      onSave={handleSave}
      initialData={modalState.editData || undefined}
    />

    <Alert
      isOpen={alert.isOpen}
      message={alert.message}
      type={alert.type}
      onClose={() => setAlert(prev => ({...prev, isOpen: false}))}
    />

  </main>);
}
