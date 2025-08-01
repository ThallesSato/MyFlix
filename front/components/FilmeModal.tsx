import {useEffect, useState} from "react"
import {FilmeDto} from "@/types/FilmeDto"
import {Filme} from "@/types/Filme"
import Rating from '@/components/Rating/Rating'

interface FilmeModalProps {
    isOpen: boolean
    onClose: () => void
    onSave: (filme: FilmeDto) => void
    initialData?: Filme
}

export default function FilmeModal({isOpen, onClose, onSave, initialData}: Readonly<FilmeModalProps>) {
    const [filme, setFilme] = useState<FilmeDto | Filme>({
        titulo: "",
        anoLancamento: 0,
        genero: "",
    } as FilmeDto)

    const [error, setError] = useState('')

    useEffect(() => {
        if (initialData) {
            setFilme(initialData)
        } else {
            setFilme({
                titulo: "",
                anoLancamento: 0,
                genero: "",
            })
        }
			setError("")
    }, [initialData, isOpen])

    const handleFilmeChange = (campo: keyof Filme, valor: string | number | boolean) => {
        setFilme(prev => ({
            ...prev,
            [campo]: valor,
        }))
    }

    const handleSubmit = () => {
        if (!filme.titulo.trim() || !filme.anoLancamento || !filme.genero.trim()) {
            setError("Preencha os campos obrigatórios. (*)")
            return
        }

        if (isNaN(filme.anoLancamento) || filme.anoLancamento < 1800) {
            setError("Informe um ano válido.")
            return
        }

        setError('')
        onSave(filme)
    }

    if (!isOpen) return null

    return (
        <div className="fixed inset-0 bg-black/50 flex justify-center items-center z-50">
            <div className="bg-[#F8F9FA] p-6 rounded-xl shadow-xl w-full max-w-md">
                <h2 className="text-xl font-semibold mb-4">
                    {initialData ? `Editar Filme: ${initialData.titulo}` : "Adicionar Filme"}
                </h2>

                {error && <div className="text-red-500 mb-3 text-sm">{error}</div>}

                <div className="mb-3">
                    <label htmlFor="titulo" className="block text-sm font-medium text-gray-700 mb-1">
                        Título*
                    </label>
                    <input
                        id="titulo"
                        className="w-full p-2 border rounded"
                        value={filme.titulo}
                        onChange={(e) => handleFilmeChange("titulo", e.target.value)}
                    />
                </div>

                <div className="mb-3">
                    <label htmlFor="anoLancamento" className="block text-sm font-medium text-gray-700 mb-1">
                        Ano de Lançamento*
                    </label>
                    <input
                        id="anoLancamento"
                        className="w-full p-2 border rounded"
                        value={filme.anoLancamento || ""}
                        type="number"
                        onChange={(e) => handleFilmeChange("anoLancamento", Number(e.target.value))}
                    />
                </div>

                <div className="mb-4">
                    <label htmlFor="genero" className="block text-sm font-medium text-gray-700 mb-1">
                        Gênero*
                    </label>
                    <input
                        id="genero"
                        className="w-full p-2 border rounded"
                        value={filme.genero}
                        onChange={(e) => handleFilmeChange("genero", e.target.value)}
                    />
                </div>

                {initialData &&
                    <>
                        <div className="mb-4">
                            <label className="flex items-center gap-2">
                                <input
                                    type="checkbox"
                                    id="status"
                                    checked={(filme as Filme).status ?? false}
                                    onChange={(e) => handleFilmeChange('status', e.target.checked)}
                                />
                                <span className="text-sm font-medium text-gray-700">Assistido</span>
                            </label>
                        </div>
                        {(filme as Filme).status &&
                            <div className="mb-4">
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Avaliação
                                </label>
                                <Rating 
                                    isEditable={true} 
                                    setRating={(e) => handleFilmeChange('nota', e)}
                                    rating={(filme as Filme).nota}
                                />
                            </div>
                        }
                    </>
                }

                <div className="flex justify-end space-x-2">
                    <button
                        onClick={onClose}
                        className="bg-[#CED4DA] hover:bg-[#ADB5BD] px-4 py-2 rounded"
                    >
                        Cancelar
                    </button>
                    <button
                        onClick={handleSubmit}
                        className="bg-[#343A40] hover:bg-[#212529] text-white px-4 py-2 rounded"
                    >
                        Salvar
                    </button>
                </div>
            </div>
        </div>
    )
}